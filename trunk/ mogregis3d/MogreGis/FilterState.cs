using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /*(internal)
     * Transmits a result state for a filter state operation
     */
    public class FilterStateResult
    {
        public enum Status
        {
            STATUS_NONE,
            STATUS_ERROR,
            STATUS_NODATA
        }

        public bool isOK() { return status != Status.STATUS_ERROR; }
        public bool isError() { return status == Status.STATUS_ERROR; }
        public bool isNoData() { return status == Status.STATUS_NODATA; }
        public bool hasData() { return isOK() && !isNoData(); }
        public Status getStatus() { return status; }
        public string getMessage() { return msg; }
        public Filter getFilter() { return filter; }


        public FilterStateResult() { }
        public FilterStateResult(FilterStateResult rhs)
        {
            this.status = rhs.status;
            this.filter = rhs.filter;
            this.msg = rhs.msg;

        }
        public FilterStateResult(Status status)
            : this(status, null, "")
        {
        }

        public FilterStateResult(Status _status, Filter _filter, string _msg)
        {
            set(_status, _filter, _msg);
        }

        public void set(Status status)
        {
            set(status, null, "");
        }
        public void set(Status status, Filter _filter)
        {
            set(status, _filter, "");
        }
        public void set(Status _status, Filter _filter, string _msg)
        {
            status = _status;
            filter = _filter;
            msg = _msg;
        }


        private Status status = Status.STATUS_NONE;
        private string msg;
        private Filter filter;
    }

    /* (no api docs)
      * Maintains the flow of data through a filter.
      */
    public abstract class FilterState
    {

        /**
         * Runs the filter logic within the contextual environment, pushing the
         * filter's output to the next filter in the chain.
         *
         * @param env
         *      Contextual compilation environment.
         * @return
         *      True if traversal succeeded, false upon error.
         */
        public abstract FilterStateResult traverse(FilterEnv env);

        /** 
         * Notifies this filter that a compilation checkpoint has been reached.
         * This supports batching/metering of data by CollectionFilters.
         *
         * @return
         *      True if traversal succeeded, false upon error.
         */
        public virtual FilterStateResult signalCheckpoint()
        {
            FilterState next = getNextState();
            return next != null ? next.signalCheckpoint() : new FilterStateResult();
        }

        /**
         * Gets the next filter state in the chain.
         */
        public FilterState getNextState()
        {
            return next_state;
        }

        /**
         * Gets a reference to the last filter environment object that passed
         * through this filter.
         *
         * @return A filter environment
         */
        public FilterEnv getLastKnownFilterEnv()
        {
            return current_env;
        }

        /**
         * Gets the report associated with this object.
         *
         * @return A Report
         */
        public Report getReport()
        {
            return report;
        }


        protected FilterState()
        {
            this.report = new Report();
        }

        protected FilterState next_state;
        protected Filter filter_prototype;
        protected string name;
        protected FilterEnv current_env;
        protected Report report;

        protected FilterState setNextState(FilterState _next_state)
        {
            //TODO: validate the the input filter is valid here
            next_state = _next_state;
            return next_state;
        }

        internal protected FilterState appendState(FilterState _state)
        {
            if (next_state != null)
            {
                next_state.appendState(_state);
            }
            else
            {
                next_state = _state;
            }

            return next_state;
        }
    }
}
