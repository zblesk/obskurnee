import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import DiscussionList from "@/components/DiscussionList.vue";
import Discussion from "@/components/Discussion.vue";
import Poll from "@/components/Poll.vue";
import PollList from "@/components/PollList.vue";

const routes = [
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
        path: "/diskusie/:discussionId",
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
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;