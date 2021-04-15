<template>
<section>
    <h1 id="tableLabel" class="page-title">Odporúčania</h1>
    <div class="main">
        <new-post mode="Recommendations" @new-post="newRecommendation"></new-post>
    </div>
    <div class="grid">
        <recommendation-card 
            v-for="rec in recommendations" 
            v-bind:key="rec.postId" 
            v-bind:post="rec" />
    </div> 
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

<style scoped>

</style>