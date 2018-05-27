namespace LogTool.Services
{
    public class CalculateService
    {
        public int AddTwoNumbers(int number1, int number2)
        {
            return number1 + number2;
        }

        public int DivideTwoNumbers(int number1, int number2)
        {
            if (number1 == 0) return 0;
            return number1 / number2;
        }
    }
}