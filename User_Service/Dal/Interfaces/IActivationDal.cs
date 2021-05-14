using System;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal.Interfaces
{
    public interface IActivationDal
    {
        /// <summary>
        /// Adds the activation object to the database
        /// </summary>
        /// <param name="activation">The activation object to add</param>
        Task Add(ActivationDto activation);

        /// <summary>
        /// Finds the activation object by the provided code
        /// </summary>
        /// <param name="code">The activation code</param>
        /// <returns>The found activation object</returns>
        Task<ActivationDto> Find(string code);

        /// <summary>
        /// Deletes the activation object which matches the uuid
        /// </summary>
        /// <param name="uuid">The uuid of the object to remove</param>
        Task Delete(Guid uuid);
    }
}