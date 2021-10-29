import { createRouter,createWebHashHistory} from "vue-router";
import Index from "../views/Index.vue";
import Category from "../views/Category.vue";
import Album from "../views/Album.vue";
import Episode from "../views/Episode.vue";
import Search from "../views/Search.vue";


const routes = [
  { path: "/", redirect: "/Index" },
  {
    path: "/Index",
	  name:"Index",
    component: Index
  },
  {
    path: "/Category",
	  name:"Category",
    component: Category
  },
  {
    path: "/Album",
	  name:"Album",
    component: Album
  },
  {
    path: "/Episode",
	  name:"Episode",
    component: Episode
  },
  {
    path: "/Search",
	  name:"Search",
    component: Search
  },  
]
const router = createRouter({
  history: createWebHashHistory(),
  routes: routes
});
export default router