using System.Text;

namespace JackpotPossibleOutcomes;
public class PossibleOutcome
{
    public List<string> ActualPossibleOutcomesPerGame { get; set; } = new();
    public string Home { get; set; } = "1";
    public string Draw { get; set; } = "X";
    public string Away { get; set; } = "2";

    public int NoofGames { get; set; } = 13;
    public int NoOfPossibleOutcomesPerGame { get; set; } = 3;

    int totalNoOfPossibleBets = 1;
    int noOfOccurrencesforEachPossibleGameOutcome = 1;
    int noOfMultiples;
    public void PrintOutcome()
    {
        Console.WriteLine("Enter Number of Football Games in the Jackpot");
        NoofGames = Convert.ToInt32(Console.ReadLine());
        ActualPossibleOutcomesPerGame.Add(Home);
        ActualPossibleOutcomesPerGame.Add(Draw);
        ActualPossibleOutcomesPerGame.Add(Away);

        CalculateTotalNoOfPossibleBets(NoofGames);
        CalculateNoOccurrencesforEachPossibleGameOutcome(NoofGames - 1);

        List<List<string>> results = new List<List<string>>();
        List<string> outcomes = new List<string>();
        noOfMultiples = 3;

        for (int i = 1; i <= NoofGames; i++)
        {
            List<string> list = new();
            
            
            for (int k = 0; k < NoOfPossibleOutcomesPerGame; k++)
            {
                for (int j = 0; j < noOfOccurrencesforEachPossibleGameOutcome; j++)
                {
                    outcomes.Add(ActualPossibleOutcomesPerGame[k]);
                    list.Add(ActualPossibleOutcomesPerGame[k]);
                }

            }
            noOfOccurrencesforEachPossibleGameOutcome = 1;
            CalculateNoOccurrencesforEachPossibleGameOutcome(NoofGames -1 - i);
            results.Add(list);
        }
        List<List<string>> results1 = new List<List<string>>();
        using FileStream fs = new FileStream(@".\data.bin", FileMode.Create);

        AddStringList(fs, results[0]);

        results1.Add(results[0]);
        for (int i = 0; i < results.Count; i++)
        {
            if (i > 0)
            {
                List<string> oldresults = results[i];
                List<string> newResults = new();
                for (int j = 0; j < noOfMultiples; j++)
                {
                    oldresults.ForEach(x =>
                    {
                        newResults.Add(x);
                    });
                }
                noOfMultiples *= 3;
                AddStringList(fs, newResults);

            }
            
        }

        fs.Close();
        using FileStream fswrite = File.Open(@".\data.bin", FileMode.Open);
        using FileStream fswrite2 = new FileStream($".\\bets {NoofGames}.txt", FileMode.Create);

        byte[] buffer = new byte[fswrite.Length - (fswrite.Length / results.Count)];
        byte[] buffer2 = new byte[results[0].Count];
        long offset1 = fswrite.Length - buffer2.LongLength;
        char[] outcome = new char[results.Count];
        for (int i = 0; i < results1[0].Count; i++)
        {
            int k = 0;                 
            for (long offset = i; offset < buffer.LongLength; offset += results1[0].Count)
            {
                fswrite.Seek(offset, SeekOrigin.Begin);
                outcome[k] = (char)fswrite.ReadByte();
                k++;
            }
            
            fswrite.Seek(offset1, SeekOrigin.Begin);
            outcome[k] = (char)fswrite.ReadByte();
            offset1++;
            Console.WriteLine(outcome);
            CreateOutcomeFile(fswrite2, outcome);
        }
        fswrite.Close();
        fswrite2.Close();
        Console.ReadLine();
    }
    private void CreateOutcomeFile(FileStream fs, char[] value)
    {
        string s = "";
        foreach (char c in value)
        {
            s += c;
        }
        s += "\n";
        byte[] buffer = new UTF8Encoding(true).GetBytes(s);
        fs.Write(buffer, 0, buffer.Length);
    }

    private void AddStringList(FileStream fs, List<string> combs)
    {
        foreach (string comb in combs)
        {
            byte[] buffer = new UTF8Encoding(true).GetBytes(comb);
            fs.Write(buffer, 0, buffer.Length);
        }
        
    }

    public void CalculateTotalNoOfPossibleBets(int noOfGames)
    { 
        for (int i = 1; i <= noOfGames; i++)
        {
            totalNoOfPossibleBets *= NoOfPossibleOutcomesPerGame;
        }

    }
    public void CalculateNoOccurrencesforEachPossibleGameOutcome(int noOfGames)
    {
        for (int i = 1; i <= noOfGames; i++)
        {
            noOfOccurrencesforEachPossibleGameOutcome *= NoOfPossibleOutcomesPerGame;
        }

    }

}
