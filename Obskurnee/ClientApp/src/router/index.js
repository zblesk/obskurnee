import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import DiscussionList from "@/components/DiscussionList.vue";
import Discussion from "@/components/Discussion.vue";
import Poll from "@/components/Poll.vue";
import PollList from "@/components/PollList.vue";
import UserList from "@/components/UserList.vue";
import RecommendationList from "@/components/RecommendationList.vue";
import Admin from "@/components/Admin.vue";
import Setup from "@/components/Setup.vue";

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