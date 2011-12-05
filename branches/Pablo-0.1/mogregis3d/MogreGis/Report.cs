using System;
using System.Collections.Generic;

namespace MogreGis
{

    public class ReportList : List<Report> { }

    /**
     * A summary of information created by some functional process.
     *
     * The reporting mechanism lets any subsystem generate information about
     * the status of its processing responsibilities by generating a Report.
     *
     * Reports can be hierarchical, each Report deliniating summary information
     * about its child reports.
     */
    public class Report
    {
        public enum State
        {
            STATE_OK = 0,
            STATE_WARNING = 10,
            STATE_ERROR = 20
        }

        #region ATRIBUTOS
        public State ReportState
        {
            set
            {
                ReportState = value;
            }

            get
            {
                return ReportState;
            }
        }

        public String Name
        {
            set
            {
                Name = value;
            }
            get
            {
                return Name;
            }
        }

        public ReportList SubReports
        {
            get
            {
                return SubReports;
            }
            private set
            {
                SubReports = value;
            }
        }

        public Properties ReportProperties
        {
            get
            {
                return ReportProperties;
            }

            private set
            {
                ReportProperties = value;
            }
        }

        public List<String> Messages
        {
            get
            {
                return Messages;
            }

            private set
            {
                Messages = value;
            }
        }

        private long firstStartTime;
        private long startTime;
        private long endTime;

        public List<double> Durations
        {
            get
            {
                return Durations;
            }

            private set
            {
                Durations = value;
            }
        }
        #endregion

        #region CONSTRUCTORES
        public Report()
        {
            ReportState = State.STATE_OK;
            firstStartTime = 0;
            startTime = 0;
            endTime = 0;
        }

        [System.Obsolete]
        public Report(Report rhs)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region METODOS
        #region TIMERS
        public void markStartTime()
        {
            startTime = startTime = System.DateTime.Now.Ticks;
            if (firstStartTime == 0)
            {
                firstStartTime = startTime;

            }
        }

        public void markEndTime()
        {
            endTime = System.DateTime.Now.Ticks;
            Durations.Add(((endTime - startTime) / 10000000));
        }

        public double getDuration()
        {
            double total = .0;
            foreach (double d in Durations)
            {
                total += d;
            }
            return total;
        }

        public double getAverageDuration()
        {
            return Durations.Count > 0 ? getDuration() / Durations.Count : 0.0;
        }

        public double getMaxDuration()
        {
            double most = .0;
            foreach (double d in Durations)
            {
                if (d > most)
                {
                    most = d;
                }
            }
            return most;
        }

        public double getMinDuration()
        {
            double least = .0;
            foreach (double d in Durations)
            {
                if (least == 0 || d < least)
                {
                    least = d;
                }
            }
            return least;
        }
        #endregion

        public void addSubReport(Report subReport)
        {
            SubReports.Add(subReport);
        }

        public void setProperty(Property p)
        {
            ReportProperties.set(p);
        }

        public void notice(string msg)
        {
            string auxiliar;
            auxiliar = "NOTICE: " + msg;
            Messages.Add(auxiliar);
        }

        public void warning(string msg)
        {
            string auxiliar;
            auxiliar = "WARNING: " + msg;
            Messages.Add(auxiliar);
            ReportState = State.STATE_WARNING;
        }

        public void error(string msg)
        {
            string auxiliar;
            auxiliar = "ERROR: " + msg;
            Messages.Add(auxiliar);
            ReportState = State.STATE_ERROR;
        }
        #endregion
    }
}
