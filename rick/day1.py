with open('day1_input') as file:
    data = file.read().splitlines()


def first_and_last_number(calibration_value: str):
    numbers = []
    for char in calibration_value:
        if char.isnumeric():
            numbers.append(char)
    return int(numbers[0] + numbers[-1])


def first_and_last_number_with_text(calibration_value: str):
    numbers_map = {
        'one': '1',
        'two': '2',
        'three': '3',
        'four': '4',
        'five': '5',
        'six': '6',
        'seven': '7',
        'eight': '8',
        'nine': '9'
    }

    text_occurences = []
    text_occurences_indices = []
    for num_text in numbers_map:
        index = calibration_value.find(num_text)
        if index != -1:
            text_occurences.append(numbers_map[num_text])
            text_occurences_indices.append(index)

    for num_text in numbers_map:
        index = calibration_value.rfind(num_text)
        if index != -1:
            text_occurences.append(numbers_map[num_text])
            text_occurences_indices.append(index)

    numbers_occurences = []
    numbers_occurences_indices = []
    for index, char in enumerate(calibration_value):
        if char.isnumeric():
            numbers_occurences.append(char)
            numbers_occurences_indices.append(index)

    first_num = None
    last_num = None

    if not text_occurences or (min(numbers_occurences_indices) < min(text_occurences_indices)):
        first_num = numbers_occurences[0]
    else:
        first_num = text_occurences[text_occurences_indices.index(min(text_occurences_indices))]

    if not text_occurences or (max(numbers_occurences_indices) > max(text_occurences_indices)):
        last_num = numbers_occurences[-1]
    else:
        last_num = text_occurences[text_occurences_indices.index(max(text_occurences_indices))]

    return int(first_num + last_num)


def part1():
    sum_of_values = 0
    for calibration_value in data:
        value = first_and_last_number(calibration_value)
        # print(f'{value} | {calibration_value}')
        sum_of_values += value

    print(f'Part 1: {sum_of_values}')


def part2():
    sum_of_values = 0
    for calibration_value in data:
        value = first_and_last_number_with_text(calibration_value)
        # print(f'{value} | {calibration_value}')
        sum_of_values += value
    print(f'Part 2: {sum_of_values}')

part1()
part2()