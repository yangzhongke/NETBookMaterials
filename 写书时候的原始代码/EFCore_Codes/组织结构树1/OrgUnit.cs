using System.Collections.Generic;

namespace 组织结构树1
{
    class OrgUnit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public OrgUnit Parent { get; set; }
        public List<OrgUnit> Children { get; set; } = new List<OrgUnit>();
    }
}
