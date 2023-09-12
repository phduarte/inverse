using System;
using System.Collections.Generic;
using System.Text;

namespace Inverse.Domain
{
    public class Comment : Entity<Guid>
    {
        public Table Table { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public static IEnumerable<Comment> FromNotes(string notes)
        {
            var lines = notes.Split('\n');

            foreach (var line in lines)
            {
                var fields = line.Split(';');

                if (fields.Length >= 3)
                {
                    yield return new Comment
                    {
                        Date = DateTime.Parse(fields[0]),
                        Author = fields[1],
                        Text = fields[2].Replace("<br />", "\n")
                    };
                }
            }
        }

        public static string ToNotes(IEnumerable<Comment> comments)
        {
            var sb = new StringBuilder();

            foreach (var comment in comments)
            {
                sb.AppendLine($"{comment.Date.ToString("yyyy-MM-dd HH:mm:ss.fff")};{comment.Author.Replace(';', ',')};{comment.Text.Replace("\n", "<br />").Replace(';', ',')}");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}-{Author}:\n{Text}";
        }
    }
}
