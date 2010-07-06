using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

#if TODO
        /**
         * Constructs a new, empty report
         */
        public Report();

        /**
         * Copy constructor
         */
        public Report(Report rhs);

        /**
         * Sets the name of this report.
         *
         * @param name
         *      A human-readable string
         */
        public void setName(string name);

        /**
         * Gets the name of this report.
         *
         * @return A human-readable string
         */
        public string getName();

        /**
         * Updates the overall state of this report. This method will only
         * set the new state if it is "worse" than the previous state (unless
         * you force a state upgrade).
         *
         * @param state
         *      New state to set
         * @param force_downgrade
         *      If true, you can override the current state with a "better"
         *      status (e.g. go from WARNING back to OK). If false (the default),
         *      attempts to upgrade the state are ignored. 
         */
        // public void setState( State state, bool force_ugprade=false );
        public void setState(State state, bool force_ugprade);

        /**
         * Gets the overall status code of the report.
         *
         * @return A status code
         */
        public State getState();

        // statistics

        /**
         * Sets the timing mode so that all the duration measurement functions
         * return information based on aggregating sub-report data instead of
         * local data.
         */
        //TODO

        /**
         * Stores the current time as the start time for duration measurement.
         */
        public void markStartTime();

        /**
         * Stores the current time as the end time for duration measurement.
         */
        public void markEndTime();

        /** 
         * Gets the total duration based on the first marked start time and the last 
         * marked end time.
         */
        public double getDuration();

        /**
         * Gets the average duration. Each time you call markEndTime()
         * a duration is stored (time from markStartTime() to markEndTime().
         * This method returns the average of all stored durations.
         *
         * @return Average duration, in seconds
         */
        public double getAverageDuration();

        /**
         * Gets the maximum stored duration. Each time you call markEndTime()
         * a duration is stored (time from markStartTime() to markEndTime().
         * This method returns the maximum of all stored durations.
         *
         * @return Maximum duration, in seconds
         */
        public double getMaxDuration();

        /**
         * Gets the minimum stored duration. Each time you call markEndTime()
         * a duration is stored (time from markStartTime() to markEndTime().
         * This method returns the minimum of all stored durations.
         *
         * @return Minimum duration, in seconds
         */
        public double getMinDuration();

        // messages

        public void notice(string msg);

        public void warning(string msg);

        public void error(string msg);

        public List<string> getMessages();

        // report nesting

        /**
         * Gets a read-only list of and sub-reports belonging to this report.
         *
         * @return Read-only list of sub-reports
         */
        public ReportList getSubReports();

        /** 
         * Adds a sub-report under this report
         *
         * @param sub_report
         *      Report to add as a sub-report to this report
         */
        public void addSubReport(Report sub_report);

        // user-defined properties

        /**
         * Sets one of the report's properties by name.
         */
        public void setProperty(Property prop);

        /**
         * Gets a collection of all this report's properties.
         */
        public Properties getProperties();



        private string name;
        private State state;
        private DateTime first_start_time, start_time, end_time;
        private List<double> durations;
        private List<string> messages;
        private ReportList sub_reports;
        private Properties properties;
#endif
    }
}
