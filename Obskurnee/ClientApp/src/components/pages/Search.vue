<template>
    <section>
        <h1 class="page-title">{{$t('menus.search')}}</h1>
        
        <book-post
            v-for="post in posts"
            v-bind:key="post.postId"
            v-bind:post="post">
        </book-post>
    </section>
</template>

<style scoped>
.books-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: var(--spacer);
    padding: var(--spacer);
}

.placeholder {
    text-align: center;
    font-style: italic;
}
</style>

<script>
import { mapActions, mapState } from "vuex";
import BookPost from "../BookPost.vue";
export default {
    name: "Search",
  components: { BookPost },
    computed: {
        ...mapState("discussions", ["discussions"]),
        // Topic posts and reposts are not eligible.
        posts: function () 
        { 
            return [].concat(...this.discussions.filter(d => d.topic == "Books").map(d => ({...d}).posts))
                .filter(p => !p.parentPostId);
        }
    },
    methods: {
    ...mapActions("discussions", ["fetchDiscussionList", "getDiscussionData"]),
    },
    async mounted() {
        await this.fetchDiscussionList();
        await Promise.all(this.discussions.map(d => this.getDiscussionData(d.discussionId)));
    }
}
</script>