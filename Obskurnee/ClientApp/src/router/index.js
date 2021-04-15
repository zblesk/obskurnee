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
    },
    {
        path: "/navrhy/:discussionId",
        name: "discussion",
        component: Discussion,
    },
    {
        path: "/hlasovania/:pollId",
        name: "poll",
        component: Poll,
    },
    {
        path: "/admin",
        name: "admin",
        component: Admin,
    },
    {
        path: "/odporucania",
        name: "recommendationlist",
        component: RecommendationList,
    },
    {
        path: "/knihy",
        name: "booklist",
        component: BookList,
    },
    {
        path: "/knihy/:bookId",
        name: "book",
        component: Book,
    },
    {
        path: "/my",
        name: "userlist",
        component: UserList,
    },
    {
        path: "/my/:email/:mode?",
        name: "user",
        component: User,
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

export default router;