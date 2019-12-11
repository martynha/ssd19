# ARIMA, orders data series
import os,sys
os.getcwd()
local_path = sys.argv[1]
#local_path = 'C:\\Users\\Marti\\Development\\courses\\SSD\\ssd2019\\Esempio1\\python_scripts'
os.chdir(local_path) 

import pandas as pd
import numpy as np
from sqlalchemy import create_engine
import common

from statsmodels.graphics.tsaplots import plot_acf, plot_pacf
import matplotlib.pyplot as plt
plt.rcParams.update({'figure.figsize':(6,4), 'figure.dpi':120})

# ---------------------------- read from sqlite database
def load_orders(db, customers):
	SQL = "SELECT time,quant FROM ordini WHERE customer IN ({})"\
		.format(customers)

	engine = create_engine('sqlite:///' + db)

	df_allorders = pd.read_sql(SQL, engine, index_col='time')

	return df_allorders

# ------------------------------ Accuracy metrics
def forecast_accuracy(forecast, actual):
    mape = np.mean(np.abs(forecast - actual)/np.abs(actual))  # MAPE
    me = np.mean(forecast - actual)             # ME
    mae = np.mean(np.abs(forecast - actual))    # MAE
    mpe = np.mean((forecast - actual)/actual)   # MPE
    rmse = np.mean((forecast - actual)**2)**.5  # RMSE
    corr = np.corrcoef(forecast, actual)[0,1]   # corr
    mins = np.amin(np.hstack([forecast[:,None], 
                              actual[:,None]]), axis=1)
    maxs = np.amax(np.hstack([forecast[:,None], 
                              actual[:,None]]), axis=1)
    minmax = 1 - np.mean(mins/maxs)             # minmax
    return({'mape':mape, 'me':me, 'mae': mae, 
            'mpe': mpe, 'rmse':rmse, 
            'corr':corr, 'minmax':minmax})

#read time series into dataframe df
#df = pd.read_csv('customer12.csv', header=0, names = ['cust12'], index_col=0)

#dbfile = 'C:\\Users\\Marti\\Development\\courses\\SSD\\ssd2019\\Esempio1\\res\\ordiniMI2018.sqlite'
dbfile = sys.argv[2]
customers = sys.argv[3]
#customers = "'cust12'"

df = load_orders(dbfile,customers)

# !pip3 install pyramid-arima
import pyramid.arima as pm
from statsmodels.tsa.arima_model import ARIMA

# Forecast next 3 months
n_forecast = 3

train = df.quant[0:-n_forecast]
test  = df.quant[-n_forecast:]

# Seasonal - fit stepwise auto-ARIMA, returns an ARIMA model
smodel = pm.auto_arima(train, start_p=1, start_q=1,
                         test='adf',
                         max_p=3, max_q=3, m=12,
                         start_P=0, seasonal=True,
                         d=None, D=1, trace=True,
                         error_action='ignore',  
                         suppress_warnings=True, 
                         stepwise=True)

# Predictions of y values based on "model", aka fitted values
yhat = smodel.predict_in_sample(start=0, end=len(train))

forecasts, confint = smodel.predict(n_periods=n_forecast, return_conf_int=True)
index_forecasts = pd.Series(range(df.index[-1]+1-n_forecast, df.index[-1]+1))

metrics = forecast_accuracy(forecasts, df.quant[-n_forecast-1:-1])
print("MAPE = {:.2f}".format(metrics['mape']) )

for i in range(0,n_forecast):
	print("Actual {:.2f} forecast {:.2f}".format(test[len(train)+i],forecasts[i]))

# make series for plotting purpose
fitted_series = pd.Series(forecasts, index=index_forecasts)
lower_series = pd.Series(confint[:, 0], index=index_forecasts)
upper_series = pd.Series(confint[:, 1], index=index_forecasts)

# Plot
plt.plot(df)
plt.plot(yhat,color='brown')
plt.plot(fitted_series, color='darkgreen')
plt.fill_between(lower_series.index, 
                 lower_series, 
                 upper_series, 
                 color='k', alpha=.15)

plt.title("SARIMA - Final Forecast of {}".format(customers))
#plt.show()

# Finally, print the chart as base64 string to the console.
common.print_figure(plt.gcf())
