
namespace Model
{
    public class User
    {
        public User(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is User user && ToTuple().Equals(user.ToTuple());
        }

        public override int GetHashCode() => ToTuple().GetHashCode();

        public (string, string) ToTuple() => (Id, Name);

        public string Id { get; }
        public string Name { get; }
    }
}
