using System;

namespace DesignPatterns.Builder
{
    public interface IReportBuilder
    {
        void SetHeader(string header);
        void SetContent(string content);
        void SetFooter(string footer);
        Report GetReport();
    }

    public class TextReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = header;
        }

        public void SetContent(string content)
        {
            _report.Content = content;
        }

        public void SetFooter(string footer)
        {
            _report.Footer = footer;
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class HtmlReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = "<h1>" + header + "</h1>";
        }

        public void SetContent(string content)
        {
            _report.Content = "<p>" + content + "</p>";
        }

        public void SetFooter(string footer)
        {
            _report.Footer = "<footer>" + footer + "</footer>";
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class Report
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }

        public override string ToString()
        {
            return $"{Header}\n{Content}\n{Footer}";
        }
    }

    public class ReportDirector
    {
        public void ConstructReport(IReportBuilder builder)
        {
            builder.SetHeader("Отчет");
            builder.SetContent("Содержимое отчета.");
            builder.SetFooter("Подвал отчета.");
        }
    }
}
