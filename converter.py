input_file = open('data.bld', 'r')
output_file = open('data_prime.bld', 'w')

for i in range(25):
    line = input_file.readline()
    output_file.write(line)

import random

population = int(input_file.readline())
for i in range(population):
    line = input_file.readline()
    data = line.split(';')
    data[1] = "{:.2f}".format(random.uniform(2, 5))
    output_file.write(data[0] + ';' + data[1] + ';' + data[2] + ';' + data[3] + ';' + data[4])