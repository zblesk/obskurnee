<template>
<section>
  <div v-if="recommendation" class="wrapper">
    <recommendation-card :recommendation="recommendation" />
    <div class="buttons buttons--padding">
      <router-link :to="{ name: 'recommendationlist' }" > 
        <button class="button-primary">◀ {{ $t("recommendations.backToRecommendations")}}</button>
      </router-link>
    </div>
  </div> 
</section>
</template>

<script>
import { mapActions } from "vuex";
import RecommendationCard from '../RecommendationCard.vue'

export default {
  name: 'SingleRecommendation',
  components: { RecommendationCard },
  data() {
      return {
        recommendationId: 0,
        recommendation: {},
      }
  },
  methods: {
    ...mapActions("recommendations", ["fetchRecommendationById"]),
  },
  async mounted() {
    this.recommendationId = this.$route.params.recommendationId;
    this.recommendation = await this.fetchRecommendationById(this.recommendationId);
  }
}
</script>
