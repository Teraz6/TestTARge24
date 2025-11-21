using Microsoft.EntityFrameworkCore;
using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;


namespace ShopTARge24.ApplicationServices.Services
{
        public class KindergartenServices : IKindergartenServices
        {
            private readonly ShopTARge24Context _context;
            private readonly IFileServices _fileServices;

            public KindergartenServices
                (
                    ShopTARge24Context context,
                    IFileServices fileServices
                )
            {
                _context = context;
                _fileServices = fileServices;
            }

            public async Task<Kindergarten> Create(KindergartenDto dto)
            {
                var domain = new Kindergarten();
                
                    domain.Id = Guid.NewGuid();
                    domain.GroupName = dto.GroupName;
                    domain.ChildrenCount = dto.ChildrenCount;
                    domain.KindergartenName = dto.KindergartenName;
                    domain.TeacherName = dto.TeacherName;
                    domain.CreatedAt = dto.CreatedAt; // DateTime - kui kaustada sellist meetodit, siis kasutaja ei saa ise midagi sisestada, kui see on soovitatav
                    domain.UpdatedAt = dto.UpdatedAt;


                await _context.Kindergarten.AddAsync(domain);
                await _context.SaveChangesAsync();

                if (dto.Files != null && dto.Files.Count > 0)
                {
                    _fileServices.UploadFilesToDatabase(dto, domain);
                }

                return domain;

            }

            public async Task<Kindergarten> Update(KindergartenDto dto)
            {
                var domain = await _context.Kindergarten.FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (domain == null)
                {
                    return null; // või throw exception saab kasutada ka siin
                }

                domain.KindergartenName = dto.KindergartenName;
                domain.GroupName = dto.GroupName;
                domain.TeacherName = dto.TeacherName;
                domain.ChildrenCount = dto.ChildrenCount;

                domain.CreatedAt = dto.CreatedAt;
                domain.UpdatedAt = dto.UpdatedAt;

            
                _context.Kindergarten.Update(domain);
                await _context.SaveChangesAsync();

                if (dto.Files != null && dto.Files.Count > 0)
                {
                    _fileServices.UploadFilesToDatabase(dto, domain);
                }

                return domain;

            }

            public async Task<Kindergarten> DetailAsync(Guid id)
            {
                var result = await _context.Kindergarten
                    .FirstOrDefaultAsync(x => x.Id == id);

                return result;
            }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var domain = await _context.Kindergarten.FirstOrDefaultAsync(x => x.Id == id);
            if (domain == null) return null;

            var images = await _context.FileToDatabases.Where(x => x.KindergartenId == id).ToArrayAsync();
            foreach (var img in images)
            {
                _context.FileToDatabases.Remove(img);
            }

            _context.Kindergarten.Remove(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<bool> RemoveImage(Guid imageId)
        {
            var image = await _context.FileToDatabases.FirstOrDefaultAsync(x => x.Id == imageId);
            if (image == null) return false;

            _context.FileToDatabases.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        //var result = await _context.Kindergarten.FirstOrDefaultAsync(x => x.Id == id);

        //var images = await _context.FileToApis
        //    .Where(x => x.KindergartenId == id)
        //    .Select(y => new FileToApiDto
        //    {
        //        Id = y.Id,
        //        KindergartenId = y.KindergartenId,
        //        ExistingFilePath = y.ExistingFilePath,
        //    }).ToArrayAsync();

        //await _fileServices.RemoveImagesFromApi(images);
        //_context.Kindergarten.Remove(result);
        //await _context.SaveChangesAsync();

        //return result;
    }
}

