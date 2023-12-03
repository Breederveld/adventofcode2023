with open('day3_input') as file:
    data = file.read().splitlines()

    numbers_coords = [] # [[(0,1), (0,2)], []]
    symbols_coords = [] # [(1,1), (1,2), ()]
    for row_num, line in enumerate(data):
        cur_number_coords = []
        for col_num, char in enumerate(line):
            if char.isnumeric():
                cur_number_coords.append((col_num, row_num))
            else:
                if cur_number_coords:
                    numbers_coords.append(cur_number_coords)
                    cur_number_coords = list()
                if char != '.':
                    symbols_coords.append((col_num, row_num))
        if cur_number_coords:
            numbers_coords.append(cur_number_coords)
    numbers = []
    for number in numbers_coords:
        value = int(''.join([data[coord[1]][coord[0]] for coord in number]))
        numbers.append({'value': value, 'coords': number})


def get_valid_neighbours(number_coords: list):
    row = number_coords[0][1]
    cols = [coord[0] for coord in number_coords]
    neighbours = []
    col_min = max(0, min(cols) - 1)
    col_max = min(len(data[row]) - 1, max(cols) + 1)
    row_min = max(0, row - 1)
    row_max = min(len(data) - 1, row + 1)

    for row in range(row_min, row_max + 1):
        for col in range(col_min, col_max + 1):
            new_coord = (col, row)
            if new_coord not in number_coords:
                neighbours.append(new_coord)
    return neighbours


def is_partnumber(neighbours):
    for neighbour in neighbours:
        if neighbour in symbols_coords:
            return True
    return False


def get_coord_number_neighbours(s_coord: tuple):
    neighbour_numbers = []
    for number in numbers:
        number_possible_neighbours = get_valid_neighbours(number['coords'])
        if s_coord in number_possible_neighbours:
            neighbour_numbers.append(number['value'])
    return neighbour_numbers


def part1():
    total = 0
    for number in numbers:
        neighbours = get_valid_neighbours(number['coords'])
        if is_partnumber(neighbours):
            total += number['value']
    print(f'Part 1: {total}')


def part2():
    # Gear = * symbol adjacent to exactly two numbers
    # Gear ratio = num1 * num2
    total_gear_ratios = 0

    for s_coord in symbols_coords:
        if data[s_coord[1]][s_coord[0]] == '*':
            number_neighbours = get_coord_number_neighbours(s_coord)
            if len(number_neighbours) == 2:
                gear_ratio = number_neighbours[0] * number_neighbours[1]
                total_gear_ratios += gear_ratio
    print(f'Part 2: {total_gear_ratios}')


part1()
part2()