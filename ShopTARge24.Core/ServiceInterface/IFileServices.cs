using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;
using System.Xml;

namespace ShopTARge24.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(KindergartenDto dto, Kindergarten domain);
        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);
        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos);
        void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);
    }
}
