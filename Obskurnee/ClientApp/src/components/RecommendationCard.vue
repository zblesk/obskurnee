<template>
  <div class="book">
    <div class="book__top">
      <div class="book__cover" v-if="recommendation.imageUrl">
        <a :href="recommendation.url" class="book__link">
          <img :src="recommendation.imageUrl" :alt="recommendation.title">
        </a>
      </div>
      <a :href="recommendation.url" class="book__link">
        <h2 class="book__title">{{ recommendation.title }}</h2>
      </a>
      <p v-if="recommendation.author" class="book__author">{{ recommendation.author }}</p>
      <p class="book__pages" v-if="recommendation.pageCount">{{$t('bookpost.numPages', [recommendation.pageCount])}}</p>
      <p class="book__owner" v-if="showName">{{$t('recommendations.recommendedBy')}}
        <user-link :userId="recommendation.ownerId" :userName="recommendation.ownerName" />
      </p>
      <p class="book__text" v-html="recommendation.renderedText"> </p>
    </div>
    <div class="book__repost" v-if="isRepostingAllowed">
        <router-link v-if="isRepostingAllowed" 
        :to="{ name: 'discussion', params: { discussionId: activeDiscussionId }, 
        query: { parentRecommendationId: recommendation.recommendationId } }">
            <button class="button-secondary button-repost">{{$t('bookpost.addToOngoing')}}</button>
        </router-link>
      </div>
  </div> 
</template>

<script>
import { mapGetters } from "vuex";
import UserLink from './UserLink.vue';
export default {
    components: { UserLink },
    name: "RecommendationCard",
    props: {
      recommendation: {
        type: Object,
        required: true,
      },
      showName: {
        type: Boolean,
        required: false,
        default: true,
      }
    },
    computed: {
      ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"]),
    }
}
</script>

<style scoped>


</style>

