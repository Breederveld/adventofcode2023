# Game 1: 7 blue, 6 green, 3 red; 3 red, 5 green, 1 blue; 1 red, 5 green, 8 blue; 3 red, 1 green, 5 blue

with open('day2_input') as file:
    data = file.read().splitlines()
    games = {}
    for game in data:
        game = game.split(': ')
        game_num = int(game[0][5:])
        sets_raw = game[1].split('; ')
        sets = []
        for set_raw in sets_raw:
            # 7 blue, 6 green, 3 red
            set = {}
            set_raw_lst = set_raw.split(', ')
            for colour in set_raw_lst:
                count, name = colour.split(' ')
                count = int(count)
                set[name] = count
            sets.append(set)
        games[game_num] = sets


def game_possible(game, available):
    for set in games[game]:
        for colour in set:
            if set[colour] > available[colour]:
                return False
    return True


def part1():
    available = {'red': 12, 'green': 13, 'blue': 14}
    total = 0

    for game in games:
        if game_possible(game, available):
            total += game
    print(f'Part 1: {total}')


def part2():
    total = 0
    for game in games:
        required = {}
        for set in games[game]:
            for colour in set:
                if colour in required:
                    required[colour] = max(required[colour], set[colour])
                else:
                    required[colour] = set[colour]

        subtotal = 1
        for count in required.values():
            subtotal *= count

        total += subtotal

    print(f'Part 2: {total}')

part1()
part2()