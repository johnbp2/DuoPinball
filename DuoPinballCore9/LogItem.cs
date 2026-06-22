namespace DuoPinballCore9
{
    public class LogItem
    {
        private string detail = "";
        private string time = DateTime.Now.ToString();

        public string Detail
        {
            get => this.detail;
            set => this.detail = value;
        }
        public string Time
        {
            get => this.time;
            set => this.time = value;
        }

        public LogItem(string v)
        {
            Detail = v;
            
        }
    }
}