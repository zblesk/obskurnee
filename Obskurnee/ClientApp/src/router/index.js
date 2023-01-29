import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/pages/Home.vue";
import RoundList from "@/components/pages/RoundList.vue";
import Discussion from "@/components/pages/Discussion.vue";
import Poll from "@/components/pages/Poll.vue";
import User from "@/components/pages/User.vue";
import UserList from "@/components/pages/UserList.vue";
import RecommendationList from "@/components/pages/RecommendationList.vue";
import Admin from "@/components/pages/Admin.vue";
import Setup from "@/components/pages/Setup.vue";
import BookList from "@/components/pages/BookList.vue";
import Book from "@/components/pages/Book.vue";
import PasswordReset from "@/components/pages/PasswordReset.vue";
import SinglePost from "@/components/pages/SinglePost.vue";
import SingleRecommendation from "@/components/pages/SingleRecommendation.vue";
import Search from "@/components/pages/Search.vue"

const routes = [
    {
        path: "/setup",
        name: "setup",
        component: Setup,
    },
    {
        path: "/",
        name: "home",
        component: Home,
    },
    {
        path: "/navrhy",
        name: "roundlist",
        component: RoundList,
        meta: { requiresAuth: true }
    },
    {
        path: "/navrhy/:discussionId",
        name: "discussion",
        component: Discussion,
        meta: { requiresAuth: true }
    },
    {
        path: "/navrhy/:discussionId/:postId",
        name: "singlepost",
        component: SinglePost,
        meta: { requiresAuth: true }
    },
    {
        path: "/hlasovania/:pollId",
        name: "poll",
        component: Poll,
        meta: { requiresAuth: true }
    },
    {
        path: "/admin",
        name: "admin",
        component: Admin,
        meta: { requiresAuth: true }
    },
    {
        path: "/odporucania",
        name: "recommendationlist",
        component: RecommendationList,
        meta: { requiresAuth: true }
    },
    {
        path: "/odporucania/:recommendationId",
        name: "singlerecommendation",
        component: SingleRecommendation,
        meta: { requiresAuth: true }
    },
    {
        path: "/knihy",
        name: "booklist",
        component: BookList,
        meta: { requiresAuth: true }
    },
    {
        path: "/knihy/:bookId",
        name: "book",
        component: Book,
        meta: { requiresAuth: true }
    },
    {
        path: "/hladanie/",
        name: "search",
        component: Search,
        meta: { requiresAuth: true }
    },
    {
        path: "/my",
        name: "userlist",
        component: UserList,
        meta: { requiresAuth: true }
    },
    {
        path: "/my/:email/:mode?",
        name: "user",
        component: User,
        meta: { requiresAuth: true }
    },
    {
        path: "/passwordreset/:userId?/:token(.*)?",
        name: "passwordreset",
        component: PasswordReset,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
      if (localStorage.getItem('jwtToken') == null) {
        next({
          name: 'home',
          params: { nextUrl: to.fullPath }
        })
      } else {
          // already authorized
          next()
        }
      } else {
          // no auth required
          next()
      }
    });
export default router;