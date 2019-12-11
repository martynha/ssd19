import os

def openFile(filePath):
    with open(filePath, "r") as file: 
        output = []
        for line in file:
            output.append(int (line))
        return output

if __name__ == "__main__":

    file1 = openFile(os.path.join('..', 'res', 'GAPreq.dat'))
    file2 = openFile(os.path.join('..', 'bin', 'GAPreq.dat'))

    file1.sort()
    file2.sort()

    print(file1 == file2)