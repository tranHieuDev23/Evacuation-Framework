input_file = open('data.bld')
output_file = open('data_prime.bld', 'w')

num_floor = int(input_file.readline())
output_file.write(str(num_floor) + '\n')

for floor in range(num_floor):
    output_file.write(input_file.readline())
    output_file.write(input_file.readline())
    output_file.write(input_file.readline())

    corridor_data = input_file.readline().split(',')
    for corridor in corridor_data:
        data = corridor.split(';')
        data[2] = float(data[2]) / 10
        output_file.write(data[0] + ';' + data[1] + ';' +
                          str(data[2]) + ';' + data[3] + ';' + data[4] + ',')
    output_file.write(input_file.readline())
    output_file.write(input_file.readline())

    num_stairway = int(input_file.readline())
    output_file.write(str(num_stairway) + '\n')
    if (num_stairway == 0):
        continue

    corridor_data = input_file.readline().split(',')
    for corridor in corridor_data:
        data = corridor.split(';')
        data[2] = float(data[2]) / 10
        output_file.write(data[0] + ';' + data[1] + ';' +
                          str(data[2]) + ';' + data[3] + ';' + data[4] + ',')

output_file.write(input_file.readline())
output_file.write(input_file.readline())
inhabitant_count = int(input_file.readline())
output_file.write(str(inhabitant_count) + '\n')
for i in range(inhabitant_count):
    output_file.write(input_file.readline())
