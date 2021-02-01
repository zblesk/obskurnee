import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/pages/Home.vue";
import RoundList from "@/components/pages/RoundList.vue";
import Discussion from "@/components/pages/Discussion.vue";
import Poll from "@/components/pages/Poll.vue";
import PollList from "@/components/pages/PollList.vue";
import User from "@/components/pages/User.vue";
import UserList from "@/components/pages/UserList.vue";
import RecommendationList from "@/components/pages/RecommendationList.vue";
import Admin from "@/components/pages/Admin.vue";
import Setup from "@/components/pages/Setup.vue";
import BookList from "@/components/pages/BookList.vue";
import DiscussionList from "@/components/pages/DiscussionList.vue";
import Book from "@/components/pages/Book.vue";

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
        path: "/diskusie",
        name: "discussionlist",
        component: DiscussionList,
    },
    {
        path: "/navrhy/:discussionId",
        name: "discussion",
        component: Discussion,
    },
    {
        path: "/hlasovania",
        name: "polllist",
        component: PollList,
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
        path: "/knihy/:bookOrder",
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
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;