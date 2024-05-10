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
import BookPost from "../BookPost.vue";
import axios from "axios";
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
  },
  methods: {

  },
  async mounted() {
      let c = await axios
          .get('/api/search/st');
  }
}
</script>