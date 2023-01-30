<template>
    <section>
        <h1 class="page-title">{{$t('menus.search')}}</h1>
        <input v-model="searchTerm" />

        <div v-if="searchTerm" class="grid">
            <book-post
                v-for="post in matches"
                v-bind:key="post.postId"
                v-bind:post="post">
            </book-post>
          <recommendation-card 
              v-for="rec in matches" 
              v-bind:key="rec.recommendationId" 
              v-bind:recommendation="rec" />
          </div>
        <span v-else>{{ $t('search.placeholder') }}</span>
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
import RecommendationCard from '../RecommendationCard.vue'
export default {
  name: "Search",
  data() {
    return {
      searchTerm: ""
    }
  },
  components: { BookPost, RecommendationCard },
  computed: {
    ...mapState("discussions", ["discussions"]),
    ...mapState("recommendations", ["recommendations"]),
    actualSearch: function() { return this.searchTerm.toLowerCase(); },
    // Topic posts and reposts are not eligible.
    posts: function () 
    { 
      return ([].concat(...this.discussions.filter(d => d.topic == "Books").map(d => ({...d}).posts))
          .filter(p => !p.parentPostId))
          .concat(this.recommendations);
    },
    matches: function ()
    {
        return this.posts.filter(p => 
            p.title.toLowerCase().includes(this.actualSearch)
            || p.author.toLowerCase().includes(this.actualSearch)
            || p.text.toLowerCase().includes(this.actualSearch))
    }
  },
  methods: {
    ...mapActions("discussions", ["fetchDiscussionList", "getDiscussionData"]),
    ...mapActions("recommendations", ["fetchRecommendationList", "newRecommendation"]),
  },
  async mounted() {
    this.fetchRecommendationList();
    await this.fetchDiscussionList();
    Promise.all(this.discussions.filter(d => d.topic == "Books").map(d => this.getDiscussionData(d.discussionId)));
  }
}
</script>