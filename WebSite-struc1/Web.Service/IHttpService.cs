using System.Threading.Tasks;

namespace Web.Service
{
    internal interface IHttpService
    {
        /// <summary>
        /// Gets data from specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the data to be returned.</typeparam>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The data.</returns>
        Task<TResult> Get<TResult>(string endPointTemplate, params string[] args);

        /// <summary>
        /// Deletes data from specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the data to be returned.</typeparam>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The data.</returns>
        Task<TResult> Delete<TResult>(string endPointTemplate, params string[] args);

        /// <summary>
        /// Posts request data to specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be returned.</typeparam>
        /// <typeparam name="TObj">The type of the object being posted.</typeparam>
        /// <param name="requestData">The request data.</param>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The result of the request.</returns>
        Task<TResult> Post<TResult, TObj>(TObj requestData, string endPointTemplate, params string[] args);

        /// <summary>
        /// Puts the specified request data.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be returned.</typeparam>
        /// <typeparam name="TObj">The type of the object being put.</typeparam>
        /// <param name="requestData">The request data.</param>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The result of the request.</returns>
        Task<TResult> Put<TResult, TObj>(TObj requestData, string endPointTemplate, params string[] args);
    }
}
