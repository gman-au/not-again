using System.Text;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class MessageFormatter : IMessageFormatter
    {
        private const string Line = "-------------";

        public string EncapsulateNotAgainMessage(string message)
        {
            var stringBuilder =
                new StringBuilder();

            stringBuilder
                .AppendLine(Line)
                .AppendLine("NotAgain Report")
                .AppendLine(message)
                .AppendLine(Line);

            return
                stringBuilder
                    .ToString();
        }
    }
}