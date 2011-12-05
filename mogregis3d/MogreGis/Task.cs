using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
     * A discrete operation that may be dispatched by a TaskManager.
     */
    public class Task
    {

        /**
         * Constructs a new task.
         *
         * @param user_data
         *      Optional user data.
         */
        public Task()
        {
            setName("Unnamed task");
            exception_state = false;
        }


        /**
         * Constructs a new task
         *
         * @param name
         *      Readable name of the task.
         * @param user_data
         *      Optional user data.
         */
        public Task(string _name)
        {
            setName(_name);
            exception_state = false;
        }

        /**
         * Gets the task name.
         *
         * @return Readable name of the task
         */
        public string getName()
        {
            return name;
        }

        /**
         * Gets the user data structure attached to this task.
         *
         * @return user data.
         */
        public object getUserData()
        {
            return user_data;
        }

        /**
         * Sets the user data structure
         *
         * @param value
         *      User data.
         */
        public void setUserData(object _user_data)
        {
            user_data = _user_data;
        }

        /**
         * Puts the task into an Exception state, meaning that something
         * unrecoverable happened during its execution.
         */
        public void setException()
        {
            exception_state = true;
        }

        /**
         * Gets whether the task was put into an exception state.
         *
         * @return True or false
         */
        public bool isInExceptionState()
        {
            return exception_state;
        }


        /** 
         * Executes the task (possibly in another thread)
         */
        public abstract void run();


        protected void setName(string _name)
        {
            name = _name;
        }


        private string name;
        private object user_data;
        private bool exception_state;
    }

    public class TaskQueue : Queue<Task> { } ;
    public class TaskList : List<Task> { };
}
