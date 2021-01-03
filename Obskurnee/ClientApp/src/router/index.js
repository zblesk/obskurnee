import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import Counter from "@/components/Counter.vue";
import FetchData from "@/components/FetchData.vue";
import DiscussionList from "@/components/DiscussionList.vue";
import Discussion from "@/components/Discussion.vue";

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
    },
    {
        path: "/Counter",
        name: "Counter",
        component: Counter,
    },
    {
        path: "/FetchData",
        name: "FetchData",
        component: FetchData,
    },
    {
        path: "/diskusie",
        name: "discussionlist",
        component: DiscussionList,
    },
    {
        path: "/diskusie/:id",
        name: "discussion",
        component: Discussion,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;