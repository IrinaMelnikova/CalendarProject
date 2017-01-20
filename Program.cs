using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
namespace CalendarProject {
    public class InputFile {
        private string path;
        public string getPath() {
            return path;
        }
        public string[] readFile() {
            return File.ReadAllLines(path);
        }
        public void setPath(string path) {
            this.path = path;
        }
        public string[] getSubstrings(string line) {
            string[] substrings = line.Split(Const.SEMICOLON_SEPARATOR);
            return substrings;
        }
        public InputFile(string path) {
            this.path = path;
        }
    }
    public class OutputFile {
        private string path;
        public OutputFile(string path) {
            this.path = path;
        }
        public void setPath(string path) {
            this.path = path;
        }
        public string getPath() {
            return path;
        }
        public void fileWrite(StreamWriter streanwriter, Field field) {
            streanwriter.WriteLine(Convert.ToString(field.getName() + Const.COLON_SEPARATOR + field.getValue()));
        }
        public void write(List<Field> list1, List<Event> list2, List<Field> list3, StreamWriter streamwriter) {
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            foreach (Field field in list1) {
                fileWrite(streamwriter, field);
            }
            foreach (Event singleEvent in list2) {
                foreach (Field field in singleEvent.events) {
                    fileWrite(streamwriter, field);
                }
            }
            foreach (Field field in list3) {
                fileWrite(streamwriter, field);
            }
            streamwriter.Close();
        }
    }
    public static class Const {
        public static char SEMICOLON_SEPARATOR = ';';
        public static string COLON_SEPARATOR = ":";
        public static string NEW_STRING_SEPARATOR = "\n";
    }
    public class Field {
        private String name;
        private String value;
        private int inputOrder;
        private int outputOrder;
        private DateTime date;
        public void setDate() {
           TimeAndDate timeAndDate = new TimeAndDate(getValue());
            date = timeAndDate.getDateAndTime();
        }
        public DateTime getDate() {
            return date; 
        }
        public int getInputOrder() { 
            return inputOrder;
        }
        public int getOutputOrder() {
            return outputOrder;
        }
        public string getName() {
            return name;
        }
        public string getValue() {
            return value;
        }
        public void setValue(string value) {
            this.value = value;
        }
        public void setInputOrder(int newInputOrder) {
            this.inputOrder = newInputOrder;
        }
        public Field(string name, string value, int inputOrder, int outputOrder) {
            this.name = name;
            this.value = value;
            this.inputOrder = inputOrder;
            this.outputOrder = outputOrder;
            setDate();
        }
    }
    public class Sort {
        private List<Field> field;
        public Sort(List<Field> field) {
            this.field = field;
        }
        public void sortList() {
            field.Sort(delegate(Field x, Field y)
            {
                return x.getOutputOrder().CompareTo(y.getOutputOrder());
            });
        }
    }
    public class Calendar {
        private Sort sortedList;
        private InputFile inputFile;
        private OutputFile outputFile;
        private List<Field> calendar = new List<Field>();
        private List<Field> calendarEnd = new List<Field>();
        private List<Event> eventList = new List<Event>();
        private static Field beginCalendar = new Field("BEGIN", "VCALENDAR", -1, 1);
        private static Field provid = new Field("PRODID", "-//Google Inc//Google Calendar 70.9054//EN", -1, 2);
        private static Field version = new Field("VERSION", "2.0", -1, 3);
        private static Field calscale = new Field("CALSCALE", "GREGORIAN", -1, 4);
        private static Field method = new Field("METHOD", "PUBLISH", -1, 5);
        private static Field endCalendar = new Field("END", "VCALENDAR", -1, 21);
        public Calendar(InputFile inputFile, OutputFile outputFile) {
            calendar = new List<Field>(new Field[] { beginCalendar, provid, version, calscale, method });
            calendarEnd = new List<Field>(new Field[] { endCalendar });
            sortedList = new Sort(calendar);
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            sortedList.sortList();
            fillEventList();
            setDate();
        }
        private void setDate()  {
            foreach (Event events in eventList)  {
                foreach(Field field in events.events)
                {
                    field.setDate();
                }
            }
        }
        public List<Event> createMultipleEventListWithValidNumberOfEvents() {
            string[] lines = inputFile.readFile();
            Event newEvent = new Event();
            for (int i = 0; i < lines.Length; i++) {
                string[] substrings = inputFile.getSubstrings(lines[i]);
                newEvent = new Event(substrings);
                eventList.Add(newEvent);
            }
            foreach (Event events in eventList) {
                Sort sortList = new Sort(events.events);
                sortList.sortList();
            }
            return eventList;
        }
        private void fillEventList() {
            eventList = createMultipleEventListWithValidNumberOfEvents();
        }
        public void write(StreamWriter streamwriter) {
            outputFile.write(calendar, eventList, calendarEnd, streamwriter);
        }
        public Calendar createCalendarByUserName(string userName) {
            Calendar filteredCalendar = new Calendar(inputFile, outputFile);
            filteredCalendar.eventList.Clear();
            foreach (Event singleEvent in eventList) {
                if(singleEvent.events.Exists(x => x.getValue() == userName)) {
                    filteredCalendar.eventList.Add(singleEvent);
                }
            }
            return filteredCalendar;
        }
        public Calendar deleteEventsBy(string userDate)  {
            Calendar calendarWithoutDeletedEvents = new Calendar(inputFile, outputFile);
            DateTime dateAndTime = DateTime.ParseExact(userDate, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            calendarWithoutDeletedEvents.eventList.Clear();
            foreach (Event singleEvent in eventList) {
                if(singleEvent.events.Exists(x => x.getName() == "DTEND")) {
                    if(singleEvent.events.Find(x => x.getName() == "DTEND").getDate() > dateAndTime) {
                        calendarWithoutDeletedEvents.eventList.Add(singleEvent);
                    }
                }
            }
            return calendarWithoutDeletedEvents;
        }
    }  
    public class Alarm {
        private Sort sortedList;
        private static Field begin = new Field("BEGIN", "VALARM", -1, 15);
        private static Field action = new Field("ACTION", "DISPLAY", -1, 16);
        private static Field description = new Field("DESCRIPTION", "-PT1440M", -1, 17);
        private static Field triger = new Field("TRIGGER", "-PT1440M", -1, 18);
        private static Field end = new Field("END", "VALARM", -1, 19);
        public List<Field> alarms = new List<Field>();
        public Alarm() {
            alarms = new List<Field>(new Field[] { begin, action, description, triger, end });
            sortedList = new Sort(alarms);
            sortedList.sortList();
        }
    }
    public class Event {
        private Alarm alarms = new Alarm();
        private Sort sortedList;
        private Field calname = new Field("X-WR-CALNAME", "", 1, 6);
        private Field beginEvent = new Field("BEGIN", "VEVENT", -1, 7);
        private Field dtstart = new Field("DTSTART", "", 2, 8);
        private Field dtend = new Field("DTEND", "", 3, 9);
        private Field dtstamp = new Field("DTSTAMP", "", 4, 10);
        private Field uid = new Field("UID", "", 5, 11);
        private Field description = new Field("DESCRIPTION", "", 6, 12);
        private Field location = new Field("lOCATION", "", 7, 13);
        private Field summary = new Field("SUMMARY", "", 8, 14);
        private Field endEvent = new Field("END", "VEVENT", -1, 20);
        public List<Field> events = new List<Field>();
        public Event()  {
            events = new List<Field>(new Field[] { calname, beginEvent, dtstart, dtend, dtstamp, uid, description, location, summary, endEvent });
            sortedList = new Sort(events);
            events.AddRange(alarms.alarms);
            sortedList.sortList();
        }
        public Event(string[] values) {
             this.events = new List<Field>(new Field[] { calname, beginEvent, dtstart, dtend, dtstamp, uid, description, location, summary, endEvent });
             this.sortedList = new Sort(events);
             this.events.AddRange(alarms.alarms);
             for (int j = 0; j < values.Length; j++)  {
                 if (this.events.Exists(x => x.getInputOrder() == j + 1)) {
                     this.events.Find(x => x.getInputOrder().Equals(j + 1)).setValue(values[j]);
                 }
             }
             this.sortedList.sortList();
         }
    }
    public class TimeAndDate {
        private string icalDate;
        private string dateString;
        public void setDate(string date) {
            this.icalDate = date;
        }
        public TimeAndDate (string date) {
            this.icalDate = date;
            dateString = deleteSymbolsInIcalDate(icalDate);
            dateAndTime = format(dateString);
        }
        public string deleteSymbolsInIcalDate(string icalDate) {
            string dateString = icalDate;
            dateString =  dateString.Replace("T", "");
            dateString = dateString.Replace("Z", "");
            return dateString;
        }
        public DateTime getDateAndTime() {
            return dateAndTime;
        }
        private DateTime dateAndTime;
        public DateTime format(string dateString) {
            DateTime date = DateTime.MinValue;
            int amountOfChar = 0;
            for (int i = 0; i < dateString.Length; i++)  {
                if (dateString.Length == 14 && dateString[i] >= '0' && dateString[i] <= '9') {
                    amountOfChar++;
                }
            }
            if (amountOfChar == 14) {
                date = DateTime.ParseExact(dateString, "yyyyddMMHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            }
            return date;
        }
    }
    public class Program {
        public static void Main(string[] args) {
            InputFile inputFile = new InputFile(@"H:\ConsoleApplication2\readfile.txt");
            OutputFile outputFile = new OutputFile(@"H:\ConsoleApplication2\writefile.txt");

            Calendar calendar = new Calendar(inputFile, outputFile);
            calendar.write(new StreamWriter(Console.OpenStandardOutput()));
            calendar.write(new StreamWriter(new FileStream(outputFile.getPath(), FileMode.Create)));

            Calendar userCalendar = calendar.createCalendarByUserName("userid@gmail.com");
            userCalendar.write(new StreamWriter(Console.OpenStandardOutput()));
            userCalendar.write(new StreamWriter(new FileStream(outputFile.getPath(), FileMode.Append)));

            Calendar calendarWithoutEvents = calendar.deleteEventsBy("10.10.2010 01:00:00");
            calendarWithoutEvents.write(new StreamWriter(Console.OpenStandardOutput()));
            calendarWithoutEvents.write(new StreamWriter(new FileStream(outputFile.getPath(), FileMode.Append)));

            Console.ReadLine();
        }
    }
}