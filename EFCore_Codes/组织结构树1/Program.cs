using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace 组织结构树1
{
	class Program
	{
		static async Task Main(string[] args)
		{
			/*
            using (TestDbContext ctx = new TestDbContext())
            {
                OrgUnit ouRoot = new OrgUnit { Name="科科国际总部"};
                ctx.OrgUnits.Add(ouRoot);

                OrgUnit ouUSA = new OrgUnit { Name="科科美国"};
                ouUSA.Parent = ouRoot;
                ctx.OrgUnits.Add(ouUSA);

                OrgUnit ouAsia = new OrgUnit { Name="科科亚太"};
                ouAsia.Parent = ouRoot;
                ctx.OrgUnits.Add(ouAsia);

                OrgUnit ouChina = new OrgUnit { Name="科科中国"};
                ouChina.Parent = ouAsia;
                ctx.OrgUnits.Add(ouChina);

                OrgUnit ouJP = new OrgUnit { Name="科科日本"};
                ouJP.Parent = ouAsia;
                ctx.OrgUnits.Add(ouJP);

                OrgUnit ouBJ = new OrgUnit { Name="科科北京"};
                ouBJ.Parent = ouChina;
                ctx.OrgUnits.Add(ouBJ);

                OrgUnit ouSH = new OrgUnit { Name = "科科上海" };
                ouSH.Parent = ouChina;
                ctx.OrgUnits.Add(ouSH);

                await ctx.SaveChangesAsync();
            }*/
			using (TestDbContext ctx = new TestDbContext())
			{
				//获取“根组织单元”
				OrgUnit ouRoot = ctx.OrgUnits.Single(o => o.Parent == null);
				Console.WriteLine(ouRoot.Name);
				PrintChildren(0, ctx, ouRoot);
			}
		}

		//输出parent的子节点
		static void PrintChildren(int indentLevel, TestDbContext ctx, OrgUnit parent)
		{
			//获取以parent为父节点的组织单元
			var children = ctx.OrgUnits.Include(o => o.Children).Where(o => o.Parent == parent);
			indentLevel++;//缩进级别
			foreach (var ou in children)
			{
				Console.Write(new string('+', indentLevel));//输出缩进
				Console.WriteLine(ou.Name);
				PrintChildren(indentLevel, ctx, ou);
			}
		}
	}
}
