<template>
  <div class="book">
    <div class="book__top" v-if="recommendation">
      <div class="book__cover" v-if="recommendation.imageUrl">
        <a :href="recommendation.url" class="book__link">
          <img :src="recommendation.imageUrl" :alt="recommendation.title">
        </a>
      </div>
      <router-link class="book__permalink" v-if="recommendation.recommendationId"
        :to="{ name: 'singlerecommendation', params: { recommendationId: recommendation.recommendationId } }">🔗
      </router-link> 
      <div>
        <a :href="recommendation.url" class="book__link">
          <h2 class="book__title" v-html="recommendation.title"></h2>
        </a>
        <p v-if="recommendation.author" class="book__author" v-html="recommendation.author"></p>
        <p class="book__pages" v-if="recommendation.pageCount">{{$t('bookpost.numPages', [recommendation.pageCount])}}</p>
        <p class="book__owner" v-if="showName">{{$t('recommendations.recommendedBy')}}
          <user-link v-if="recommendation.ownerId" :userId="recommendation.ownerId" :userName="recommendation.ownerName" />
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
  </div> 
</template>

<script>
import { mapGetters } from "vuex";
import UserLink from './UserLink.vue';
import MarkdownEditor from "./MarkdownEditor.vue";
export default {
  components: { UserLink, MarkdownEditor },
  name: "SearchResultRecommendation",
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
    ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"])
  }
}
</script>
