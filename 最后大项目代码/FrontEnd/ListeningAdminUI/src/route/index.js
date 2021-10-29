import { createRouter,createWebHashHistory} from "vue-router";
import CategoryList from "../views/CategoryList.vue";
import CategoryAdd from "../views/CategoryAdd.vue";
import CategoryEdit from "../views/CategoryEdit.vue";
import AlbumList from "../views/AlbumList.vue";
import AlbumAdd from "../views/AlbumAdd.vue";
import AlbumEdit from "../views/AlbumEdit.vue";
import EpisodeList from "../views/EpisodeList.vue";
import EpisodeAdd from "../views/EpisodeAdd.vue";
import EpisodeEdit from "../views/EpisodeEdit.vue";
import Login from "../views/Login.vue";
import AdminUserAdd from "../views/AdminUserAdd.vue";
import AdminUserEdit from "../views/AdminUserEdit.vue";
import AdminUserList from "../views/AdminUserList.vue";


const routes = [
  { path: "/", redirect: "/CategoryList" },
  {
    path: "/CategoryList",
	  name:"CategoryList",
    component: CategoryList
  },
  {
    path: "/CategoryAdd",
	  name:"CategoryAdd",
    component: CategoryAdd
  },
  {
    path: "/CategoryEdit",
	  name:"CategoryEdit",
    component: CategoryEdit
  },
  {
    path: "/AlbumList",
	  name:"AlbumList",
    component: AlbumList
  },
  {
    path: "/AlbumAdd",
	  name:"AlbumAdd",
    component: AlbumAdd
  },
  {
    path: "/AlbumEdit",
	  name:"AlbumEdit",
    component: AlbumEdit
  },
  {
    path: "/EpisodeList",
	  name:"EpisodeList",
    component: EpisodeList
  },
  {
    path: "/EpisodeAdd",
	  name:"EpisodeAdd",
    component: EpisodeAdd
  },
  {
    path: "/EpisodeEdit",
	  name:"EpisodeEdit",
    component: EpisodeEdit
  },
  {
    path: "/Login",
	  name:"Login",
    component: Login
  },
  {
    path: "/AdminUserAdd",
	  name:"AdminUserAdd",
    component: AdminUserAdd
  },
  {
    path: "/AdminUserEdit",
	  name:"AdminUserEdit",
    component: AdminUserEdit
  },
  {
    path: "/AdminUserList",
	  name:"AdminUserList",
    component: AdminUserList
  }
]
const router = createRouter({
  history: createWebHashHistory(),
  routes: routes
});
export default router