<template>
<section>
    <h1 id="tableLabel" class="page-title">{{$t('menus.recs')}}</h1>
    <div class="main">
        <new-post mode="Recommendations" @new-post="onNewRecommendation">{{$t('recommendations.addNew')}}</new-post>
    </div>
    <div class="grid">
        <recommendation-card 
            v-for="rec in recommendations" 
            v-bind:key="rec.recommendationId" 
            v-bind:recommendation="rec" />
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
        onNewRecommendation(rec)
        {
            const onErr = this.$notifyError;
            this.newRecommendation(rec)
                .then(() => this.emitter.emit('clear-post'))
                .catch(function (error) {
                    console.log(error);
                    onErr(this.$t('recommendations.failedToAddRec'));
                });
        }
    },
    mounted() {
        this.fetchRecommendationList();
    }
}
</script>