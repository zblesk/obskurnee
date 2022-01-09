<template>
<section>
    <router-link :to="{ name: 'recommendationlist' }" >◀ {{ $t("recommendations.backToRecommendations")}}</router-link>
    <div v-if="recommendation" class="grid">
      <recommendation-card :recommendation="recommendation" />
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
