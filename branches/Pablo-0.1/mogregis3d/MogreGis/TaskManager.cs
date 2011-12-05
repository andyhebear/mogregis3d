using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MogreGis
{

    /* (internal)
     * Object that a TaskManager uses to run a Task in a separate thread.
     */
    public class TaskThread : Task
    {

        public enum State
        {
            STATE_READY,
            STATE_RUNNING,
            STATE_RESULT_READY,
            STATE_EXIT
        };


        public TaskThread(int _id, AutoResetBlock _ab)
        {
            id = _id;
            activity_block = _ab;
            state = State.STATE_READY;
            //start();
        }

        public int getID()
        {
            return id;
        }

        public State getState();
        public void runTask(Task task);
        public void dispose();

        public double getResultDuration();
        public Task removeTask();

        public override void run()
        {
            throw new NotImplementedException();
        }


        private void setState(State value)
        {
            throw new NotImplementedException();
        }

        private int id;
        private Task task;
        private AutoResetBlock activity_block;
        private bool done;
        private AutoResetBlock run_block;
        private State state;
        private OpenThreads::Mutex state_mutex;
        private osg.Timer_t start, end;
    }

    public class TaskThreadList : List<TaskThread> { }
    public class TaskThreadMap : Dictionary<int, TaskThread> { }


    /**
     * Dispatches Task objects and tracks their progress.
     *
     * A TaskManager maintains a queue of Task instances, and dispatches them one
     * by one, each in its own thread (TaskThread).
     *
     * Each time you call wait(), the TaskManager will check for completed tasks and
     * dispatch pending tasks if there are threads available. You can then call
     * getNextCompletedTask() to dequeue a task that has finished.
     *
     * So the usage pattern is:
     *
     * <pre>
     *   task_man->queueTask( task1 );
     *   task_man->queyeTask( task2 );
     *   ...
     *   while( task_man->wait() )
     *   {
     *      osg::ref_ptr<Task> completed_task = task_man->getNextCompletedTask();
     *      ...
     *   }
     * </pre>
     */
    public class TaskManager
    {

        /**
         * Constructs a new task manager. The task manager will allocate a pool of
         * working threads equal to the number of logical processors found on the
         * system.
         */
        public TaskManager();

        /** 
         * Constructs a new task manager.
         *
         * @param max_threads
         *      Maximum number of threads to allocate for running Tasks.
         */
        public TaskManager(int max_threads);


        /**
         * Enqueue a task for execution. The task will run when you call wait(),
         * the task has reached the front of the queue, and there is a working
         * thread available.
         *
         * @param task
         *      Task to put on the dispatch queue
         */
        public void queueTask(Task task);

        /**
         * Keep calling this method until it returns false.
         *
         * Calling wait() checks for completed tasks, checks for pending tasks and
         * dispatching them to available threads, and then blocks until the first
         * running task completes.
         *
         * @param timeout_ms
         *      Time (in milliseconds) to block waiting for something to happen.
         *
         * @return True if there are still more tasks pending or running; False if
         *         all known tasks have completed.
         */
        public bool wait();
        public bool wait(ulong timeout_ms);

        /**
         * Checks whether there are any uncompleted tasks.
         *
         * @return True if there are still tasks that have not completed.
         */
        public bool hasMoreTasks();

        /**
         * Gets the number of tasks still under the task manager's control.
         *
         * @return Number of tasks remaining
         */
        public uint getNumTasks();

        /**
         * Gets the next completed task and returns it to the user
         *
         * @returns A completed task wrapped in a reference pointer.
         */
        public Task getNextCompletedTask();

        /**
         * Removes any pending tasks (tasks that have not yet been dispatched)
         * from the queue.
         */
        public void cancelPendingTasks();


        private bool multi_threaded;
        private TaskThreadList threads;
        private TaskQueue pending_tasks;
        private TaskQueue completed_tasks;
        private int num_running_tasks;
        private Mutex q_mutex;
        private AutoResetBlock activity_block;

        private void init(int num_threads);
        private void update();
    }
}