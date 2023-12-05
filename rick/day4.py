with open('day4_input') as file:
    data = file.read().splitlines()

cards = []
for row in data:
    # Card   1: 66 92  4 54 39 76 49 27 61 56 | 66 59 85 54 61 86 37 49  6 18 81 39  4 56  2 48 76 72 71 25 27 67 10 92 13
    card, numbers = row.split(':')
    card_num = int(card.split(' ')[-1])
    winning, draw = numbers.split('|')
    winning = [int(num) for num in winning.split()]
    draw = [int(num) for num in draw.split()]
    cards.append({'card_num': card_num, 'draw': draw, 'winning': winning})


def get_matches(card):
    matches = 0
    for num in card['draw']:
        if num in card['winning']:
            matches += 1
    return matches


def part1():
    total = 0
    for card in cards:
        matches = get_matches(card)
        if matches > 0:
            total += 2 ** (matches - 1)
    print(f'Part 1: {total}')


def part2():
    queue = cards.copy()
    cards_handled = 0
    while queue:
        card = queue.pop()
        cards_handled += 1
        matches = get_matches(card)
        for new_card_num in range(card['card_num'] + 1, min(card['card_num'] + 1 + matches, len(cards) + 1)):
            queue.append(next(card for card in cards if card['card_num'] == new_card_num))
    print(f'Part 2: {cards_handled}')


part1()
part2()