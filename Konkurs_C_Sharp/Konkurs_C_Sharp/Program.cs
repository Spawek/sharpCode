using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konkurs_C_Sharp
{
    public static class EnumerableExtensions
    {
        public static T MaxElement<T, R>(this IEnumerable<T> container, Func<T, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) > 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }

        public static T MaxElement<T>(this IEnumerable<T> container) where T : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var maxElem = enumerator.Current;
            var maxVal = maxElem;

            while (enumerator.MoveNext())
            {
                var currVal = enumerator.Current;

                if (currVal.CompareTo(maxVal) > 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }

        public static T MinElement<T, R>(this IEnumerable<T> container, Func<T, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var minElem = enumerator.Current;
            var minVal = valuingFoo(minElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(minVal) < 0)
                {
                    minVal = currVal;
                    minElem = enumerator.Current;
                }
            }

            return minElem;
        }

        public static T MinElement<T>(this IEnumerable<T> container) where T : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var minElem = enumerator.Current;
            var minVal = minElem;

            while (enumerator.MoveNext())
            {
                var currVal = enumerator.Current;

                if (currVal.CompareTo(minVal) < 0)
                {
                    minVal = currVal;
                    minElem = enumerator.Current;
                }
            }

            return minElem;
        }
    }

    class Program
    {
        static string inputDirectory = @"C:\maciek\sharpCode\Konkurs_C_Sharp\Konkurs_C_Sharp\Dane\Input\";
        static string outputDirectory = @"C:\maciek\sharpCode\Konkurs_C_Sharp\Konkurs_C_Sharp\Dane\Output\";

        class Ride
        {
            public int rideNo;

            public int rowStart;
            public int colStart;

            public int rowEnd;
            public int colEnd;

            public int earliestStart;
            public int latestEnd;

            public int distance()
            {
                return Math.Abs(rowEnd - rowStart) + Math.Abs(colStart - colEnd);
            }

            public override string ToString()
            {
                return $"{rideNo}:  ({rowStart}, {colStart}) -> ({rowEnd}, {colEnd})   [{earliestStart}, {latestEnd}]";
            }
        }

        class InputStructure
        {
            public int rows;
            public int columns;
            public int vehiclesNo;
            public int numberOfRides;
            public int perRideBonus;
            public int numberOfSteps;
            public List<Ride> rides;
        };

        class RidesPerVehicle
        {
            public Vehicle vehicle;
            public List<Ride> rides;
        }

        class OutputStructure
        {
            public Dictionary<Vehicle, RidesPerVehicle> ridesPerVehicle = new Dictionary<Vehicle, RidesPerVehicle>();
        };

        static InputStructure Parse(string fileName)
        {
            Console.WriteLine($"parsing file started: {fileName}");

            var inputStructure = new InputStructure();

            using (var sr = new StreamReader(fileName))
            {
                var firstLine = sr.ReadLine().Split(' ');
                inputStructure.rows = Int32.Parse(firstLine[0]);
                inputStructure.columns = Int32.Parse(firstLine[1]);
                inputStructure.vehiclesNo = Int32.Parse(firstLine[2]);
                inputStructure.numberOfRides = Int32.Parse(firstLine[3]);
                inputStructure.perRideBonus = Int32.Parse(firstLine[4]);
                inputStructure.numberOfSteps = Int32.Parse(firstLine[5]);
                inputStructure.rides = new List<Ride>();

                string line;
                int rideNo = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var splitted = line.Split(' ');
                    var ride = new Ride();
                    ride.rideNo = rideNo++;
                    ride.rowStart = Int32.Parse(splitted[0]);
                    ride.colStart = Int32.Parse(splitted[1]);
                    ride.rowEnd = Int32.Parse(splitted[2]);
                    ride.colEnd = Int32.Parse(splitted[3]);
                    ride.earliestStart = Int32.Parse(splitted[4]);
                    ride.latestEnd = Int32.Parse(splitted[5]);
                    inputStructure.rides.Add(ride);
                }
            }

            Console.WriteLine($"parsing file finished: {fileName}");
            return inputStructure;
        }

        static void WriteResultToFile(OutputStructure outputStructure, string fileName)
        {
            Console.WriteLine($"writing result file started: {fileName}");
            using (var sw = new StreamWriter(fileName))
            {
                var sorted = outputStructure.ridesPerVehicle
                    .Select(x => x.Value)
                    .OrderBy(x => x.vehicle.vehicleNo)
                    .ToList();
                foreach (var vehicleRides in sorted)
                {
                    sw.Write($"{vehicleRides.rides.Count} ");
                    sw.WriteLine(String.Join(" ", vehicleRides.rides.Select(x => x.rideNo)));
                }
            }

            Console.WriteLine($"writing result file finished: {fileName}");
        }

        class Vehicle
        {
            public int vehicleNo;

            public int row = 0;
            public int col = 0;
            public int availableAtTime = 0;
            public int bonusPointsIfOnTime;

            public bool canFulfill(Ride ride)
            {
                var beThereAt = availableAtTime + distanceToStart(ride);
                var finishAt = beThereAt + ride.distance();
                return finishAt <= ride.latestEnd;
            }

            public int distanceToStart(Ride ride)
            {
                return Math.Abs(col - ride.colStart) + Math.Abs(row - ride.rowStart);
            }

            public int pointsFor(Ride ride)
            {
                // CAN BE REMOVED FOR OPTIMIZATION!
                if (!canFulfill(ride))
                    throw new ApplicationException();
                // UNTIL HERE

                var beThereAt = availableAtTime + distanceToStart(ride);
                int bonus = (beThereAt <= ride.earliestStart) ? bonusPointsIfOnTime : 0;

                return bonus + ride.distance();
            }

            public int timeSpent(Ride ride)
            {
                var beThereAt = availableAtTime + distanceToStart(ride);
                var afterWaiting = Math.Max(beThereAt, ride.earliestStart);
                return afterWaiting + ride.distance() - availableAtTime;
            }

            public void executeRide(Ride ride)
            {
                availableAtTime = availableAtTime + timeSpent(ride);
                row = ride.rowEnd;
                col = ride.colEnd;
            }
        }

        static OutputStructure Solve(InputStructure inputStructure, bool metropolisHax = false)
        {
            var os = new OutputStructure();

            var vehicles = new List<Vehicle>();
            for (int i = 0; i < inputStructure.vehiclesNo; i++)
                vehicles.Add(new Vehicle() { bonusPointsIfOnTime = inputStructure.perRideBonus, vehicleNo = i });

            var allVehiclesAtAll = vehicles.ToList();

            var ridesLeft = inputStructure.rides.ToList();

            List<Ride> ridesForTheEnd = null;
            if (metropolisHax)
            {
                ridesLeft = ridesLeft
                    .Where(x => x.rowStart > 1600)
                    .Where(x => x.rowEnd < 4000)
                    .Where(x => x.colStart > 0)
                    .Where(x => x.colEnd < 2500)
                    .ToList();

                ridesForTheEnd = inputStructure.rides.ToList();
                ridesForTheEnd.RemoveAll(x => ridesLeft.Any(y => x.rideNo == y.rideNo));
            }

            while (ridesLeft.Count > 0 && vehicles.Count > 0)
            {
                var currVehicle = vehicles.MinElement(x => x.availableAtTime);
                var availableRides = ridesLeft.Where(ride => currVehicle.canFulfill(ride)).ToList();

                if (availableRides.Count == 0 && ridesForTheEnd != null)
                {
                    ridesLeft.AddRange(ridesForTheEnd);
                    availableRides = ridesLeft.Where(ride => currVehicle.canFulfill(ride)).ToList();  // HAX
                    ridesForTheEnd = null;
                }

                if (availableRides.Count == 0)
                {
                    vehicles.Remove(currVehicle);
                    continue;
                }

                var bestRide = availableRides.MaxElement(ride => ((double)currVehicle.pointsFor(ride)) / currVehicle.timeSpent(ride));
                var pointsForRide = currVehicle.pointsFor(bestRide);
                var timeSpent = currVehicle.timeSpent(bestRide);

                ridesLeft.Remove(bestRide);

                if (currVehicle.pointsFor(bestRide) == 0)
                    throw new ApplicationException();

                currVehicle.executeRide(bestRide);
                if (!os.ridesPerVehicle.ContainsKey(currVehicle))
                    os.ridesPerVehicle.Add(currVehicle, new RidesPerVehicle() { vehicle = currVehicle, rides = new List<Ride>()});
                os.ridesPerVehicle[currVehicle].rides.Add(bestRide);
            }

            return os;
        }

        static void ProcessFile(string baseFileName)
        {
            Console.WriteLine($"processing file started {baseFileName}");

            var input = Parse(inputDirectory + baseFileName + ".in");
            var output = Solve(input, baseFileName.Contains("metropolis"));
            WriteResultToFile(output, outputDirectory + baseFileName + ".out");

            Console.WriteLine($"processing file finished {baseFileName}");
        }

        static void Main(string[] args)
        {
            //ProcessFile("a_example");
            //ProcessFile("b_should_be_easy");
            //ProcessFile("c_no_hurry");
            //ProcessFile("d_metropolis");
            ProcessFile("e_high_bonus");
        }
    }
}
