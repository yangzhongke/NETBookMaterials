using EFcoreOneToOneBug;
using Microsoft.EntityFrameworkCore;

using MyDbContext ctx = new MyDbContext();
A a1 = new A() { Id=Guid.NewGuid(),Name="ABC"};
ctx.A.Add(a1);
ctx.SaveChanges();
A a2 = ctx.A.Include(x=>x.B).Single(x=>x.Id==a1.Id);
a1.B = new B() { Id = Guid.NewGuid(), Age = 18 };
ctx.SaveChanges();