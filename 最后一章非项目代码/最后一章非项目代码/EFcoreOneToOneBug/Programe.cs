using EFcoreOneToOneBug;

using MyDbContext ctx = new MyDbContext();
A a1 = new A();
ctx.A.Add(a1);
ctx.SaveChanges();
A a2 = 