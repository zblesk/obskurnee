import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import RoundList from "@/components/RoundList.vue";
import Discussion from "@/components/Discussion.vue";
import Poll from "@/components/Poll.vue";
import PollList from "@/components/PollList.vue";
import UserList from "@/components/UserList.vue";
import RecommendationList from "@/components/RecommendationList.vue";
import Admin from "@/components/Admin.vue";
import Setup from "@/components/Setup.vue";
import BookList from "@/components/BookList.vue";
import DiscussionList from "@/components/DiscussionList.vue";
import BookLargeCard from "@/components/BookLargeCard.vue";

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
        path: "/knihy/:bookId",
        name: "book",
        component: BookLargeCard,
    },
    {
        path: "/my",
        name: "users",
        component: UserList,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;