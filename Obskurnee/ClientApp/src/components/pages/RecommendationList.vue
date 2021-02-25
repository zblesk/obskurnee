<template>
<section>
    <h1 id="tableLabel">RecommendationList</h1>
    <new-post mode="Recommendations" @new-post="newRecommendation"></new-post>
    <recommendation-card 
        v-for="rec in recommendations" 
        v-bind:key="rec.postId" 
        v-bind:post="rec" />
</section>
</template>


<script>
import { mapActions, mapState } from "vuex";
import RecommendationCard from '../RecommendationCard.vue'
import NewPost from '../NewPost.vue'
export default {
  components: { RecommendationCard, NewPost },
    name: "RecommendationList",
    computed: {
        ...mapState("recommendations", ["recommendations"]),
    },
    methods: {
        ...mapActions("recommendations", ["fetchRecommendationList", "newRecommendation"]),
    },
    mounted() {
        this.fetchRecommendationList();
    }
}
</script>