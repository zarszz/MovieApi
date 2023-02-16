namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseUserDto
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        
        public static ResponseUserDto FromEntity(Entities.User user) => new ResponseUserDto
        {
            ID = user.Id,
            Name = user.Name,
            Email = user.Email,
            Avatar = user.Avatar
        };
    }
}