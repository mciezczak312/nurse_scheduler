# [0;11] - max 23 zmiany
# [12] - max 20 zmian
# [13;15] - max 13 zmian
# 0 - nie chce LATE

import csv
import json

DAY_SHIFT = 0
EARLY_SHIFT = 1
LATE_SHIFT = 2
NIGHT_SHIFT = 3
REST_SHIFT = 4

SHIFTS_NUMBER = 5
WEEKS_NUMBER = 5
DAYS_IN_WEEK = 7
NURSES_NUMBER = 16

hardConstraints = dict((i, 0) for i in range(1, 12))
softConstraints = dict((i, 0) for i in range(1, 7))

def subfinder(mylist, pattern):
    matches = []
    for i in range(len(mylist)):
        if mylist[i] == pattern[0] and mylist[i:i+len(pattern)] == pattern:
            matches.append(pattern)
    return matches

# kazda zmiana obsadzona
def firstConstraint(shifts):
    def normal(shifts):
        if shifts != 3:
            hardConstraints[1] += 1
    def night(shifts):
        if shifts != 1:
            hardConstraints[1] += 1
    def weekend(shifts):
        if shifts != 2:
            hardConstraints[1] += 1

    transposedShifts = [list(i) for i in zip(*shifts)]
    shiftsCount = [sum(x) for x in transposedShifts]
    for i in range(len(shiftsCount)):
        shiftNumber = i % (SHIFTS_NUMBER * DAYS_IN_WEEK)
        if shiftNumber in [0,1,2,5,6,7,10,11,12,15,16,17,20,21,22]:
            normal(shiftsCount[i])
        if shiftNumber in [3,8,13,18,23,28,33]:
            night(shiftsCount[i])
        if shiftNumber in [25,26,27,30,31,32]:
            weekend(shiftsCount[i])

# jedna zmiana jednego dnia
def secondConstraint(shifts):
    flatShifts = [item for sublist in shifts for item in sublist]
    shiftsArrays = [flatShifts[i:i+5] for i in range(0,len(flatShifts),5)]
    shiftsCount = [sum(x) for x in shiftsArrays]
    hardConstraints[2] += len(shiftsCount) - shiftsCount.count(1)

# spelnienie wymiarow godzinowych z mozliwoscia jednokrotnego przekroczenia o 4h -> ilosc zmian tak jak wyliczona u gory
def thirdConstraint(shifts):
    for index, nurse in enumerate(shifts):
        nurseArray = [item for sublist in [nurse[i:i+4] for i in range(0, len(nurse), 5)] for item in sublist]    # usuniecie rest shift
        count = sum(nurseArray)
        if index in range(0, 12):
            if count > 23:
                hardConstraints[3] += 1
        if index == 12:
            if count > 20:
                hardConstraints[3] += 1
        if index in range(13, 16):
            if count > 13:
                hardConstraints[3] += 1

# max 3 nocne zmiany
def fourthConstraint(shifts):
    nightShifts = []
    for nurseShifts in shifts:
        count = 0
        for i in range(len(nurseShifts)):
            shiftNumber = i % (SHIFTS_NUMBER * DAYS_IN_WEEK)
            if shiftNumber in [3,8,13,18,23,28,33]:
                count += nurseShifts[i]
        nightShifts.append(count)

    for nightShift in nightShifts:
        hardConstraints[4] += 0 if nightShift - 3 <= 0 else 1

# 2 wolne weekendy -> piatkowa nocna i cala sobota i niedziela maja byc wolne (9 zmian) x 2
def fifthConstraint(shifts):
    for nurse in shifts:
        nurseArray = [item for sublist in [nurse[i:i+4] for i in range(0, len(nurse), 5)] for item in sublist]    # usuniecie rest shift
        weekends = [nurseArray[i:i+8] for i in range(19, len(nurseArray), 28)]
        freeWeekends = 0
        for weekend in weekends:
            if sum(weekend) == 0:
                freeWeekends += 1
        if freeWeekends < 2:
            hardConstraints[5] += 1

# 42h odpoczynku po 2+ zmianach nocnych -> po 2 ostatnich zmianach nocnych musza byc 2 zmiany restu
def sixthConstraint(shifts):
    def mapper(shift):
        if shift == [0,0]:
            return 'o'
        if shift == [0,1]:
            return 'r'
        if shift == [1,0]:
            return 'n'
    for nurse in shifts:
        nurseArray = [nurse[i:i+2] for i in range(3, len(nurse), 5)]
        nurseArray = [mapper(i) for i in nurseArray]
        hardConstraints[6] += 1 if (len(subfinder(nurseArray, ['n','n','r','o'])) + len(subfinder(nurseArray, ['n','n','o'])) + len(subfinder(nurseArray, ['n','n','r','n']))) else 0

# 11h przerwy po zmianie -> po poznej nie mozna wczesnej ani dziennej
def seventhConstraint(shifts):
    for nurse in shifts:
        nurseLateArray = [nurse[i] for i in range(2, len(nurse), 5)]  # lista zmian late
        nurseEarlyDayArray = [1 if sum(nurse[i:i+2]) >= 1 else 0 for i in range(0, len(nurse), 5)]
        nurseEarlyDayArray = nurseEarlyDayArray[1:len(nurseEarlyDayArray)]   # lista zmian day lub early poczynajÄc od dnia drugiego
        nurseEarlyDayArray.append(0)    # dodanie, zeby dla ostatniego dnia sie zgadzalo
        nurseArray = [None] * (len(nurseLateArray) + len(nurseEarlyDayArray))
        nurseArray[::2] = nurseLateArray
        nurseArray[1::2] = nurseEarlyDayArray   # tutaj se zmerdzowalem naprzemiennie obie te tablice
        nurseArray = [nurseArray[i:i+2] for i in range(0, len(nurseArray), 2)]  # tutaj pakuje w pary
        hardConstraints[7] += 1 if [1,1] in nurseArray else 0   # jezeli po late jest early lub day to constraint zlamany

# 14h przerwy po nocnej -> po zmianie nocnej albo kolejna nocna, albo rest
def eightConstraint(shifts):
    for nurse in shifts:
        nurseNightsArray = [nurse[i] for i in range(3, len(nurse), 5)]
        nurseNightRestArray = [1 if sum(nurse[i:i+2]) == 1 else 0 for i in range(3, len(nurse), 5)]
        nurseNightRestArray = nurseNightRestArray[1:len(nurseNightRestArray)]
        nurseNightRestArray.append(1)
        nurseArray = [None] * (len(nurseNightsArray) + len(nurseNightRestArray))
        nurseArray[::2] = nurseNightsArray
        nurseArray[1::2] = nurseNightRestArray
        nurseArray = [nurseArray[i:i+2] for i in range(0, len(nurseArray), 2)]
        hardConstraints[8] += 1 if [1,0] in nurseArray else 0

# max 3 nocnych pod rzÄd -> nie moze byc 4 lub wiecej nocnych pod rzÄd
def ninthConstraint(shifts):
    for nurse in shifts:
        nurseArray = [item for sublist in [nurse[i:i+1] for i in range(3, len(nurse), 5)] for item in sublist]
        hardConstraints[9] += 1 if len(subfinder(nurseArray, [1,1,1,1])) else 0

# max 6 dni pracy pod rzÄd -> nie moĹźe byÄ okresu 7 dni bez zadnego resta
def tenthConstraint(shifts):
    for nurse in shifts:
        nurseArray = [item for sublist in [nurse[i:i+1] for i in range(4, len(nurse), 5)] for item in sublist]
        hardConstraints[10] += 1 if len(subfinder(nurseArray, [0,0,0,0,0,0,0])) else 0

# zerowa pielegniarka bez late shifts
def eleventhConstraint(shifts):
    nurse = shifts[0]
    nurseLateArray = [nurse[i] for i in range(2, len(nurse), 5)]  # lista zmian late
    if sum(nurseLateArray):
        hardConstraints[11] += 1


# wolne lub pracujace weekendy
def firstSoftConstraint(shifts):
    for nurse in shifts:
        nurseFridaysLateNightArray = [sum(nurse[i:i+2]) for i in range(22, len(nurse), 35)]
        nurseSaturdayRestArray = [nurse[i] for i in range(28, len(nurse), 35)]
        nurseSundayRestArray = [nurse[i] for i in range(34, len(nurse), 35)]
        nurseWeekendRestArray = [sum(x) for x in list(zip(nurseSaturdayRestArray, nurseSundayRestArray))]
        array = list(zip(nurseFridaysLateNightArray, nurseWeekendRestArray))
        for weekend in array:
            if not (weekend[1] == 0 or weekend == (0,2)):
                softConstraints[1] += 1
    softConstraints[1] *= 1000

# 2-3 nocne pod rzad jesli caly lub niepelny etat
def secondSoftConstraint(shifts):
    for index, nurse in enumerate(shifts):
        if (index <= 12):
            nightsArray = [item for sublist in [nurse[i:i+1] for i in range(3, len(nurse), 5)] for item in sublist]
            singleNights = len(subfinder(nightsArray, [0,1,0]))
            overThreeNights = len(subfinder(nightsArray, [1,1,1,1]))
            softConstraints[2] += singleNights + overThreeNights
    softConstraints[2] *= 1000

# 4-5 zmian w tygodniu przy calym lub niepelnym etacie
def thirdSoftConstraint(shifts):
    for index, nurse in enumerate(shifts):
        if (index <= 12):
            weeks = [item for item in [nurse[i:i+35] for i in range(0, len(nurse), 35)]]
            for week in weeks:
                del week[4::5]
            weeks = [sum(item) for item in weeks]
            for index2, working_days in enumerate(weeks):
                if working_days < 4:
                    weeks[index2] = 4 - working_days
                elif working_days > 5:
                    weeks[index2] = working_days - 5
                else:
                    weeks[index2] = 0
            softConstraints[3] += sum(weeks)
    softConstraints[3] *= 10

# 2-3 zmiany w tygodniu dla pracownikow na pol etatu i mniej
def fourthSoftConstraint(shifts):
    for index, nurse in enumerate(shifts):
        if (index > 12):
            weeks = [item for item in [nurse[i:i+35] for i in range(0, len(nurse), 35)]]
            for week in weeks:
                del week[4::5]
            weeks = [sum(item) for item in weeks]
            for index2, working_days in enumerate(weeks):
                if working_days < 2:
                    weeks[index2] = 2 - working_days
                elif working_days > 3:
                    weeks[index2] = working_days - 3
                else:
                    weeks[index2] = 0
            softConstraints[4] += sum(weeks)
    softConstraints[4] *= 10

# 5-6 zmian pod rząd jak na cały etat lub niepelny
def fifthSoftConstraint(shifts):
    for index, nurse in enumerate(shifts):
        if (index <= 12):
            nurseArray = [item for sublist in [nurse[i:i+1] for i in range(4, len(nurse), 5)] for item in sublist]
            oneShift = len(subfinder(nurseArray, [1,0,1]))
            twoShifts = len(subfinder(nurseArray, [1,0,0,1]))
            threeShifts = len(subfinder(nurseArray, [1,0,0,0,1]))
            softConstraints[5] += 3 * oneShift + 2 * twoShifts + threeShifts
    softConstraints[5] *= 10

# unikac nocnej po wczesnej
def sixthSoftConstraint(shifts):
    for nurse in shifts:
        nurseEarlyArray = [nurse[i] for i in range(0, len(nurse), 5)]
        nurseNightArray = [nurse[i] for i in range(3, len(nurse), 5)]  # lista zmian night
        nurseNightArray = nurseNightArray[1:len(nurseNightArray)]   # lista zmian night poczynajÄc od dnia drugiego
        nurseNightArray.append(0)    # dodanie, zeby dla ostatniego dnia sie zgadzalo
        nurseArray = [None] * (len(nurseNightArray) + len(nurseEarlyArray))
        nurseArray[::2] = nurseEarlyArray
        nurseArray[1::2] = nurseNightArray   # tutaj sobie zmerdzowalem naprzemiennie obie te tablice
        nurseArray = [nurseArray[i:i+2] for i in range(0, len(nurseArray), 2)]  # tutaj pakuje w pary
        softConstraints[6] += 1 if [1,1] in nurseArray else 0   # jezeli po early jest night to constraint zlamany



nursesArray = []


with open('schedule.csv', newline='') as csvfile:
    reader = csv.reader(csvfile, delimiter=',', quotechar='|')
    for row in reader:
        nursesArray.append(row)

# for nurse in nursesArray:
#     print(nurse)

nursesArray = [list(map(int, nurse[1:-1])) for nurse in nursesArray]
# for nurse in nursesArray:
#     print(nurse)

firstConstraint(nursesArray)
secondConstraint(nursesArray)
thirdConstraint(nursesArray)
fourthConstraint(nursesArray)
fifthConstraint(nursesArray)
sixthConstraint(nursesArray)
seventhConstraint(nursesArray)
eightConstraint(nursesArray)
ninthConstraint(nursesArray)
tenthConstraint(nursesArray)
eleventhConstraint(nursesArray)

firstSoftConstraint(nursesArray)
secondSoftConstraint(nursesArray)
thirdSoftConstraint(nursesArray)
fourthSoftConstraint(nursesArray)
fifthSoftConstraint(nursesArray)
sixthSoftConstraint(nursesArray)

jsonarray = json.dumps(hardConstraints)
jsonarray2 = json.dumps(softConstraints)
print(jsonarray)
print(';;')
print(jsonarray2)