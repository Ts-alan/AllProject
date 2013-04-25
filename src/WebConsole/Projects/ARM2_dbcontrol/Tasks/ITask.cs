using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// Интерфейс задачи 
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Инициализация полей
        /// </summary>
        void InitFields();

        /// <summary>
        /// Проверка заполненных полей
        /// </summary>
        /// <returns>истина в случае успеха</returns>
        bool ValidateFields();

        /// <summary>
        /// Возвращает текущее состояние всех полей
        /// </summary>
        /// <returns>сущность-задача</returns>
        TaskUserEntity GetCurrentState();

        /// <summary>
        /// Загружает поля в соответствии с задачей-сущностью
        /// </summary>
        /// <param name="task">задача-сущность</param>
        void LoadState(TaskUserEntity task);
      
    }
}
