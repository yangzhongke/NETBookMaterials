import { createRouter,createWebHashHistory} from "vue-router";
import Test from "../views/Test.vue";
import Login from "../views/Login.vue";
const routes = [{path: "/", redirect: "/Test"},
  {path: "/Test",name:"Test",component: Test},
  {path: "/Login",name:"Login",component: Login}]
const router = createRouter({history: createWebHashHistory(),routes: routes});
export default router