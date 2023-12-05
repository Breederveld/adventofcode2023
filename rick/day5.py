with open('day5_input') as file:
    data = file.read().splitlines()

seeds = list(map(int, data[0].split(':')[1].split()))
maps = {}
for line in range(2, len(data)):
    if data[line] != '' and not data[line][0].isnumeric():
        map_name = data[line].split(' ')[0]
        map_list = []
        lineline = line + 1
        while lineline < len(data) and data[lineline] != '':
            dest_start, source_start, length = map(int, data[lineline].split())
            map_list.append({'dest_start': dest_start, 'source_start': source_start, 'length': length})
            lineline += 1
        maps[map_name] = map_list


def get_seed_location(seed: int):
    category = 'seed'
    value = seed
    while category != 'location':
        map_name = next(key for key in maps if key.startswith(category))
        map_list = maps[map_name]
        mapping = next((mapping for mapping in map_list if mapping['source_start'] <= value <= (mapping['source_start'] + mapping['length'])), None)
        if mapping is not None:
            value = mapping['dest_start'] + (value - mapping['source_start'])
        category = map_name.split('-')[2]
    return value


def get_seed_from_location(location: int):
    category = 'location'
    value = location
    while category != 'seed':
        map_name = next(key for key in maps if key.endswith(category))
        map_list = maps[map_name]
        mapping = next((mapping for mapping in map_list if mapping['dest_start'] <= value <= (mapping['dest_start'] + mapping['length'])), None)
        if mapping is not None:
            value = mapping['source_start'] + (value - mapping['dest_start'])
        category = map_name.split('-')[0]
    return value


def seed_in_seeds(seed):
    for start_seed in range(0, len(seeds), 2):
        if seeds[start_seed] <= seed <= seeds[start_seed] + seeds[start_seed + 1]:
            return True
    return False


def part1():
    locations = []
    for seed in seeds:
        loc = get_seed_location(seed)
        locations.append(loc)
    print(f'Part 1: {min(locations)}')


def part2():
    # start at lowest location, see if that gets to a seed in my list
    map_list = list(sorted(maps['humidity-to-location'], key=lambda x: x['dest_start']))
    for mapping in map_list:
        for location_num in range(mapping['dest_start'], mapping['dest_start'] + mapping['length']):
            if location_num > 46018775: # overflowed output screen buffer 1st time :'(
                seed = get_seed_from_location(location_num)
                if seed_in_seeds(seed):
                    print(f'Part 2: {location_num}')
                    break


#part1()
part2()